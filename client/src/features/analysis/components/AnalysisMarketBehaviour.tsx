import type { MetricForAnalysis } from '@/features/analysis/types'
import AnalysisMetricSection from './AnalysisMetricSection'

type AnalysisMarketBehaviourProps = {
  metrics: MetricForAnalysis[]
}

export default function AnalysisMarketBehaviour({
  metrics,
}: AnalysisMarketBehaviourProps) {
  return (
    <AnalysisMetricSection
      title="Market Behaviour"
      metrics={metrics}
      emptyTitle="No market behaviour"
      emptyDescription="Behaviour metrics will appear once candle data is available for this range."
    />
  )
}
