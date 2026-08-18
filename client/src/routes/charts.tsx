import { useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute, useNavigate } from '@tanstack/react-router'
import {
  boundsQueryOptions,
  candlesQueryOptions,
} from '@/api/market-data/queries'
import type { DatasetBoundsDto } from '@/api/market-data/types'
import ChartsPage from '@/features/charts/components/ChartsPage'
import { toChartSeriesModel } from '@/features/charts/mappers'
import type { ApiClient } from '@/api/httpClient'
import {
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import {
  parseTimeframe,
  type Timeframe,
} from '@/shared/constants/market'

export const Route = createFileRoute('/charts')({
  validateSearch: (search: Record<string, unknown>) => ({
    timeframe: parseTimeframe(search.timeframe, DEFAULT_TIMEFRAME),
  }),
  loaderDeps: ({ search: { timeframe } }) => ({ timeframe }),
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

    await context.queryClient.ensureQueryData(
      candlesQueryOptions(context.apiClient, {
        symbol: DEFAULT_SYMBOL,
        timeframe: deps.timeframe,
        startDate: bounds.firstOpenTime,
        endDate: bounds.lastOpenTime,
      }),
    )
  },
  component: ChartsRoute,
})

type ChartsLoadedViewProps = {
  apiClient: ApiClient
  bounds: DatasetBoundsDto
  timeframe: Timeframe
  onTimeframeChange: (timeframe: Timeframe) => void
}

function ChartsLoadedView({
  apiClient,
  bounds,
  timeframe,
  onTimeframeChange,
}: ChartsLoadedViewProps) {
  const { data: candles } = useSuspenseQuery(
    candlesQueryOptions(apiClient, {
      symbol: DEFAULT_SYMBOL,
      timeframe,
      startDate: bounds.firstOpenTime,
      endDate: bounds.lastOpenTime,
    }),
  )

  const series = toChartSeriesModel(candles, DEFAULT_SYMBOL, timeframe)

  return (
    <ChartsPage
      timeframe={timeframe}
      onTimeframeChange={onTimeframeChange}
      series={series}
    />
  )
}

function ChartsRoute() {
  const { apiClient } = Route.useRouteContext()
  const { timeframe } = Route.useSearch()
  const navigate = useNavigate({ from: '/charts' })

  function handleTimeframeChange(nextTimeframe: Timeframe) {
    void navigate({
      search: (prev) => ({ ...prev, timeframe: nextTimeframe }),
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
        <ChartsPage
          timeframe={timeframe}
          onTimeframeChange={handleTimeframeChange}
          series={null}
        />
      ) : (
        <ChartsLoadedView
          apiClient={apiClient}
          bounds={bounds}
          timeframe={timeframe}
          onTimeframeChange={handleTimeframeChange}
        />
      )}
    </main>
  )
}
