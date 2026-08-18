import type {
  MarketBehaviourDto,
  MarketSummaryDto,
  PriceChangeDto,
  ProbabilityDto,
  RiskAnalysisDto,
} from '@/api/analysis/types'
import type {
  AnalysisOverviewModel,
  MetricForAnalysis,
  PriceChangeForAnalysis,
  ProbabilityBucketForAnalysis,
} from '@/features/analysis/types'
import { formatCurrency } from '@/shared/formatting/formatCurrency'
import { formatNumber } from '@/shared/formatting/formatNumber'
import {
  formatPercentage,
  formatUnsignedPercentage,
} from '@/shared/formatting/formatPercentage'

function formatCandles(count: number) {
  return `${formatNumber(count)} ${count === 1 ? 'candle' : 'candles'}`
}

export function toMarketSummaryMetrics(
  summary: MarketSummaryDto | null,
): MetricForAnalysis[] {
  if (!summary) {
    return []
  }

  return [
    {
      id: 'latest-close',
      label: 'Latest Close',
      value: formatCurrency(summary.latestClose),
    },
    {
      id: 'highest-high',
      label: 'Highest Price',
      value: formatCurrency(summary.highestHigh),
    },
    {
      id: 'lowest-low',
      label: 'Lowest Price',
      value: formatCurrency(summary.lowestLow),
    },
    {
      id: 'candle-count',
      label: 'Candle Count',
      value: formatNumber(summary.candleCount),
    },
  ]
}

export function toPriceChangeForAnalysis(
  priceChange: PriceChangeDto | null,
): PriceChangeForAnalysis | null {
  if (!priceChange) {
    return null
  }

  const tone: PriceChangeForAnalysis['tone'] =
    priceChange.changePercent > 0
      ? 'success'
      : priceChange.changePercent < 0
        ? 'danger'
        : 'neutral'

  return {
    changePercent: priceChange.changePercent,
    formattedChangePercent: formatPercentage(priceChange.changePercent),
    oldestClose: priceChange.oldestClose,
    latestClose: priceChange.latestClose,
    formattedOldestClose: formatCurrency(priceChange.oldestClose),
    formattedLatestClose: formatCurrency(priceChange.latestClose),
    tone,
  }
}

export function toRiskAnalysisMetrics(
  riskAnalysis: RiskAnalysisDto | null,
): MetricForAnalysis[] {
  if (!riskAnalysis) {
    return []
  }

  return [
    {
      id: 'historical-volatility',
      label: 'Historical Volatility',
      value: formatCurrency(riskAnalysis.volatility),
    },
    {
      id: 'maximum-drawdown',
      label: 'Maximum Drawdown',
      value: formatUnsignedPercentage(riskAnalysis.maximumDrawdownPercent),
      tone: 'danger',
    },
  ]
}

export function toMarketBehaviourMetrics(
  behaviour: MarketBehaviourDto | null,
): MetricForAnalysis[] {
  if (!behaviour) {
    return []
  }

  return [
    {
      id: 'average-candle-range',
      label: 'Average Candle Range',
      value: formatCurrency(behaviour.averageCandleRange),
    },
    {
      id: 'longest-bullish-streak',
      label: 'Longest Bullish Streak',
      value: formatPercentage(behaviour.longestBullishStreakReturnPercent),
      hint: formatCandles(behaviour.longestBullishStreakPeriods),
      tone: 'success',
    },
    {
      id: 'longest-bearish-streak',
      label: 'Longest Bearish Streak',
      value: formatPercentage(behaviour.longestBearishStreakReturnPercent),
      hint: formatCandles(behaviour.longestBearishStreakPeriods),
      tone: 'danger',
    },
  ]
}

export function toProbabilityBuckets(
  probability: ProbabilityDto | null,
): ProbabilityBucketForAnalysis[] {
  if (!probability) {
    return []
  }

  return [
    {
      id: 'bullish',
      label: 'Bullish',
      count: probability.upCandles,
      formattedCount: formatNumber(probability.upCandles),
      percent: probability.probabilityUp,
      formattedPercent: formatUnsignedPercentage(probability.probabilityUp),
      tone: 'success',
    },
    {
      id: 'bearish',
      label: 'Bearish',
      count: probability.downCandles,
      formattedCount: formatNumber(probability.downCandles),
      percent: probability.probabilityDown,
      formattedPercent: formatUnsignedPercentage(probability.probabilityDown),
      tone: 'danger',
    },
    {
      id: 'neutral',
      label: 'Neutral',
      count: probability.neutralCandles,
      formattedCount: formatNumber(probability.neutralCandles),
      percent: probability.probabilityNeutral,
      formattedPercent: formatUnsignedPercentage(probability.probabilityNeutral),
      tone: 'neutral',
    },
  ]
}

export function toAnalysisOverviewModel(
  summary: MarketSummaryDto | null,
  priceChange: PriceChangeDto | null,
  riskAnalysis: RiskAnalysisDto | null,
  marketBehaviour: MarketBehaviourDto | null,
  probability: ProbabilityDto | null,
): AnalysisOverviewModel {
  return {
    marketSummary: toMarketSummaryMetrics(summary),
    priceChange: toPriceChangeForAnalysis(priceChange),
    riskAnalysis: toRiskAnalysisMetrics(riskAnalysis),
    marketBehaviour: toMarketBehaviourMetrics(marketBehaviour),
    probability: toProbabilityBuckets(probability),
  }
}
