export type MetricForAnalysis = {
  id: string
  label: string
  value: string
  hint?: string
  tone?: 'neutral' | 'success' | 'danger' | 'accent'
}

export type PriceChangeForAnalysis = {
  changePercent: number
  formattedChangePercent: string
  oldestClose: number
  latestClose: number
  formattedOldestClose: string
  formattedLatestClose: string
  tone: 'success' | 'danger' | 'neutral'
}

export type ProbabilityBucketForAnalysis = {
  id: 'bullish' | 'bearish' | 'neutral'
  label: string
  count: number
  formattedCount: string
  percent: number
  formattedPercent: string
  tone: 'success' | 'danger' | 'neutral'
}

export type AnalysisOverviewModel = {
  marketSummary: MetricForAnalysis[]
  priceChange: PriceChangeForAnalysis | null
  riskAnalysis: MetricForAnalysis[]
  marketBehaviour: MetricForAnalysis[]
  probability: ProbabilityBucketForAnalysis[]
}
