import type { MetricForAnalysis } from '@/features/analysis/types'
import AnalysisMetricSection from './AnalysisMetricSection'

type AnalysisRiskAnalysisProps = {
  metrics: MetricForAnalysis[]
}

export default function AnalysisRiskAnalysis({
  metrics,
}: AnalysisRiskAnalysisProps) {
  return (
    <AnalysisMetricSection
      title="Risk Analysis"
      metrics={metrics}
      emptyTitle="No risk analysis"
      emptyDescription="Risk analysis will appear once enough candles are available for this range."
    />
  )
}
