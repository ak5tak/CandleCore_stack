import AnalysisOverview from '@/features/analysis/components/AnalysisOverview'
import type { AnalysisOverviewModel } from '@/features/analysis/types'
import type { Period, Timeframe } from '@/shared/constants/market'

type AnalysisPageProps = {
  timeframe: Timeframe
  period: Period
  onTimeframeChange: (timeframe: Timeframe) => void
  onPeriodChange: (period: Period) => void
  model: AnalysisOverviewModel
}

export default function AnalysisPage({
  timeframe,
  period,
  onTimeframeChange,
  onPeriodChange,
  model,
}: AnalysisPageProps) {
  return (
    <AnalysisOverview
      timeframe={timeframe}
      period={period}
      onTimeframeChange={onTimeframeChange}
      onPeriodChange={onPeriodChange}
      model={model}
    />
  )
}
