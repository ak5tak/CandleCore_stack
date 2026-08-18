import { useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { dashboardOverviewQueryOptions } from '@/api/dashboard/queries'
import { boundsQueryOptions } from '@/api/market-data/queries'
import type { DatasetBoundsDto } from '@/api/market-data/types'
import DashboardPage from '@/features/dashboard/components/DashboardPage'
import { toDashboardOverviewModel } from '@/features/dashboard/mappers'
import type { DashboardOverviewModel } from '@/features/dashboard/types'
import type { ApiClient } from '@/api/httpClient'
import {
  DEFAULT_LOOKBACK,
  DEFAULT_PERIOD,
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import { getPeriodDateRange, parsePeriod, type Period } from '@/shared/constants/market'

const emptyModel: DashboardOverviewModel = {
  statistics: [],
  dataset: null,
}

export const Route = createFileRoute('/')({
  validateSearch: (search: Record<string, unknown>) => ({
    period: parsePeriod(search.period, DEFAULT_PERIOD),
  }),
  loaderDeps: ({ search: { period } }) => ({ period }),
  loader: async ({ context, deps }) => {
    const boundsParams = {
      symbol: DEFAULT_SYMBOL,
      timeframe: DEFAULT_TIMEFRAME,
    }

    const bounds = await context.queryClient.ensureQueryData(
      boundsQueryOptions(context.apiClient, boundsParams),
    )

    if (!bounds) {
      return
    }

    const range = getPeriodDateRange(
      deps.period,
      new Date(bounds.lastOpenTime),
      new Date(bounds.firstOpenTime),
    )

    await context.queryClient.ensureQueryData(
      dashboardOverviewQueryOptions(context.apiClient, {
        symbol: DEFAULT_SYMBOL,
        timeframe: DEFAULT_TIMEFRAME,
        startDate: range.startDate,
        endDate: range.endDate,
        lookback: DEFAULT_LOOKBACK,
      }),
    )
  },
  component: DashboardRoute,
})

type DashboardLoadedViewProps = {
  apiClient: ApiClient
  bounds: DatasetBoundsDto
  period: Period
  onPeriodChange: (period: Period) => void
}

function DashboardLoadedView({
  apiClient,
  bounds,
  period,
  onPeriodChange,
}: DashboardLoadedViewProps) {
  const range = getPeriodDateRange(
    period,
    new Date(bounds.lastOpenTime),
    new Date(bounds.firstOpenTime),
  )

  const { data: overviewDto } = useSuspenseQuery(
    dashboardOverviewQueryOptions(apiClient, {
      symbol: DEFAULT_SYMBOL,
      timeframe: DEFAULT_TIMEFRAME,
      startDate: range.startDate,
      endDate: range.endDate,
      lookback: DEFAULT_LOOKBACK,
    }),
  )

  const { statistics, dataset } = toDashboardOverviewModel(overviewDto)

  return (
    <DashboardPage
      period={period}
      onPeriodChange={onPeriodChange}
      statistics={statistics}
      dataset={dataset}
    />
  )
}

function DashboardRoute() {
  const { apiClient } = Route.useRouteContext()
  const { period } = Route.useSearch()
  const navigate = useNavigate({ from: '/' })

  function handlePeriodChange(nextPeriod: Period) {
    void navigate({
      search: (prev) => ({ ...prev, period: nextPeriod }),
      resetScroll: false,
    })
  }

  const { data: bounds } = useSuspenseQuery(
    boundsQueryOptions(apiClient, {
      symbol: DEFAULT_SYMBOL,
      timeframe: DEFAULT_TIMEFRAME,
    }),
  )

  return (
    <main className="w-full py-8 text-left">
      {!bounds ? (
        <DashboardPage
          period={period}
          onPeriodChange={handlePeriodChange}
          statistics={emptyModel.statistics}
          dataset={emptyModel.dataset}
        />
      ) : (
        <DashboardLoadedView
          apiClient={apiClient}
          bounds={bounds}
          period={period}
          onPeriodChange={handlePeriodChange}
        />
      )}
    </main>
  )
}
