export type DashboardOverviewDto = {
  symbol: string
  interval: string
  candleCount: number
  latestClose: number
  highestHigh: number
  lowestLow: number
  athDistancePercent: number
  changePercent: number | null
  totalVolume: number
  firstCandleTime: string | null
  lastCandleTime: string | null
}

export type DashboardOverviewParams = {
  symbol: string
  timeframe: string
  startDate?: string
  endDate?: string
  lookback?: number
}
