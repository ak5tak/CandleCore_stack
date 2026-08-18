import ChartOverview from '@/features/charts/components/ChartOverview'
import type { ChartSeriesModel } from '@/features/charts/types'
import type { Timeframe } from '@/shared/constants/market'

type ChartsPageProps = {
  timeframe: Timeframe
  onTimeframeChange: (timeframe: Timeframe) => void
  series: ChartSeriesModel | null
}

export default function ChartsPage({
  timeframe,
  onTimeframeChange,
  series,
}: ChartsPageProps) {
  return (
    <ChartOverview
      timeframe={timeframe}
      onTimeframeChange={onTimeframeChange}
      series={series}
    />
  )
}
