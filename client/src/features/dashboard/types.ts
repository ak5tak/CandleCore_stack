export type StatisticForOverview = {
  id: string
  label: string
  value: string
  hint?: string
  tone?: 'neutral' | 'success' | 'danger' | 'accent'
}

export type DatasetSummaryForOverview = {
  symbol: string
  displayName: string
  interval: string
  rangeLabel: string
  candleCount: number
  formattedCandleCount: string
  lastCandleAt: string | null
  formattedLastCandleAt: string
  formattedVolume: string
}

export type DashboardOverviewModel = {
  statistics: StatisticForOverview[]
  dataset: DatasetSummaryForOverview | null
}
