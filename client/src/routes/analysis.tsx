import { useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { analysisOverviewQueryOptions } from '@/api/analysis/queries'
import { boundsQueryOptions } from '@/api/market-data/queries'
import type { DatasetBoundsDto } from '@/api/market-data/types'
import AnalysisPage from '@/features/analysis/components/AnalysisPage'
import { toAnalysisOverviewModel } from '@/features/analysis/mappers'
import type { AnalysisOverviewModel } from '@/features/analysis/types'
import type { ApiClient } from '@/api/httpClient'
import {
  DEFAULT_LOOKBACK,
  DEFAULT_PERIOD,
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import {
  getPeriodDateRange,
  parsePeriod,
  parseTimeframe,
  type Period,
  type Timeframe,
} from '@/shared/constants/market'

const emptyModel: AnalysisOverviewModel = {
  marketSummary: [],
  priceChange: null,
  riskAnalysis: [],
  marketBehaviour: [],
  probability: [],
}

export const Route = createFileRoute('/analysis')({
  validateSearch: (search: Record<string, unknown>) => ({
    timeframe: parseTimeframe(search.timeframe, DEFAULT_TIMEFRAME),
    period: parsePeriod(search.period, DEFAULT_PERIOD),
  }),
  loaderDeps: ({ search: { timeframe, period } }) => ({
    timeframe,
    period,
  }),
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
      analysisOverviewQueryOptions(context.apiClient, {
        symbol: DEFAULT_SYMBOL,
        timeframe: deps.timeframe,
        startDate: range.startDate,
        endDate: range.endDate,
        lookback: DEFAULT_LOOKBACK,
      }),
    )
  },
  component: AnalysisRoute,
})

type AnalysisLoadedViewProps = {
  apiClient: ApiClient
  bounds: DatasetBoundsDto
  timeframe: Timeframe
  period: Period
  onTimeframeChange: (timeframe: Timeframe) => void
  onPeriodChange: (period: Period) => void
}

function AnalysisLoadedView({
  apiClient,
  bounds,
  timeframe,
  period,
  onTimeframeChange,
  onPeriodChange,
}: AnalysisLoadedViewProps) {
  const range = getPeriodDateRange(
    period,
    new Date(bounds.lastOpenTime),
    new Date(bounds.firstOpenTime),
  )

  const { data: overview } = useSuspenseQuery(
    analysisOverviewQueryOptions(apiClient, {
      symbol: DEFAULT_SYMBOL,
      timeframe,
      startDate: range.startDate,
      endDate: range.endDate,
      lookback: DEFAULT_LOOKBACK,
    }),
  )

  const model = overview
    ? toAnalysisOverviewModel(
        overview.summary,
        overview.priceChange,
        overview.riskAnalysis,
        overview.marketBehaviour,
        overview.probability,
      )
    : emptyModel

  return (
    <AnalysisPage
      timeframe={timeframe}
      period={period}
      onTimeframeChange={onTimeframeChange}
      onPeriodChange={onPeriodChange}
      model={model}
    />
  )
}

function AnalysisRoute() {
  const { apiClient } = Route.useRouteContext()
  const { timeframe, period } = Route.useSearch()
  const navigate = useNavigate({ from: '/analysis' })

  function handleTimeframeChange(nextTimeframe: Timeframe) {
    void navigate({
      search: (prev) => ({ ...prev, timeframe: nextTimeframe }),
      resetScroll: false,
    })
  }

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
        <AnalysisPage
          timeframe={timeframe}
          period={period}
          onTimeframeChange={handleTimeframeChange}
          onPeriodChange={handlePeriodChange}
          model={emptyModel}
        />
      ) : (
        <AnalysisLoadedView
          apiClient={apiClient}
          bounds={bounds}
          timeframe={timeframe}
          period={period}
          onTimeframeChange={handleTimeframeChange}
          onPeriodChange={handlePeriodChange}
        />
      )}
    </main>
  )
}
