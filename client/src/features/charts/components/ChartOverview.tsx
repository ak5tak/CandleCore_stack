import type { ChartSeriesModel } from '@/features/charts/types'
import ChartCandles from '@/features/charts/components/ChartCandles'
import ChartToolbar from '@/features/charts/components/ChartToolbar'
import EmptyState from '@/shared/components/ui/EmptyState'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'
import type { Timeframe } from '@/shared/constants/market'

type ChartOverviewProps = {
  timeframe: Timeframe
  onTimeframeChange: (timeframe: Timeframe) => void
  series: ChartSeriesModel | null
}

export default function ChartOverview({
  timeframe,
  onTimeframeChange,
  series,
}: ChartOverviewProps) {
  return (
    <Stack gap={8}>
      <Stack
        direction="row"
        gap={4}
        className="flex-wrap items-end justify-between"
      >
        <Stack gap={2}>
          <Heading as={1}>Charts</Heading>
          <Text muted>
            Explore the full Bitcoin candle history with zoom and pan.
          </Text>
        </Stack>

        <ChartToolbar
          timeframe={timeframe}
          onTimeframeChange={onTimeframeChange}
        />
      </Stack>

      {series && series.points.length > 0 ? (
        <Stack gap={4}>
          <Text size="sm" muted>
            {series.symbol} · {series.interval} · {series.rangeLabel}
          </Text>
          <ChartCandles points={series.points} />
        </Stack>
      ) : (
        <EmptyState
          title="No candles for this interval"
          description="Candle history will appear here once market data is available."
        />
      )}
    </Stack>
  )
}
