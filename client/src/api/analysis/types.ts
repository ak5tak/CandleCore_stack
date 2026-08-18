export type MarketSummaryDto = {
  symbol: string
  interval: string
  candleCount: number
  latestClose: number
  averageClose: number
  highestHigh: number
  lowestLow: number
}

export type PriceChangeDto = {
  symbol: string
  interval: string
  lookback: number
  oldestClose: number
  latestClose: number
  changePercent: number
}

export type RiskAnalysisDto = {
  symbol: string
  interval: string
  lookback: number
  averageClose: number
  volatility: number
  maximumDrawdownPercent: number
}

export type MarketBehaviourDto = {
  symbol: string
  interval: string
  lookback: number
  averageCandleRange: number
  longestBullishStreakPeriods: number
  longestBullishStreakReturnPercent: number
  longestBearishStreakPeriods: number
  longestBearishStreakReturnPercent: number
}

export type ProbabilityDto = {
  symbol: string
  interval: string
  lookback: number
  upCandles: number
  downCandles: number
  neutralCandles: number
  probabilityUp: number
  probabilityDown: number
  probabilityNeutral: number
}

export type AnalysisSymbolTimeframeParams = {
  symbol: string
  timeframe: string
  startDate?: string
  endDate?: string
}

export type AnalysisOverviewDto = {
  summary: MarketSummaryDto | null
  priceChange: PriceChangeDto | null
  riskAnalysis: RiskAnalysisDto | null
  marketBehaviour: MarketBehaviourDto | null
  probability: ProbabilityDto | null
}

export type OverviewParams = AnalysisSymbolTimeframeParams & {
  lookback?: number
}
