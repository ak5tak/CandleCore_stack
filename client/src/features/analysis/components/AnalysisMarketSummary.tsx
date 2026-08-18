import type { MetricForAnalysis } from '@/features/analysis/types'
import AnalysisMetricSection from './AnalysisMetricSection'

type AnalysisMarketSummaryProps = {
  metrics: MetricForAnalysis[]
}

export default function AnalysisMarketSummary({
  metrics,
}: AnalysisMarketSummaryProps) {
  return (
    <AnalysisMetricSection
      title="Market Summary"
      metrics={metrics}
      emptyTitle="No market summary"
      emptyDescription="Summary metrics will appear once candle data is available for this range."
    />
  )
}
