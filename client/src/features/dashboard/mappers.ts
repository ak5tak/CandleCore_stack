import type { DashboardOverviewDto } from '@/api/dashboard/types'
import type {
  DashboardOverviewModel,
  DatasetSummaryForOverview,
  StatisticForOverview,
} from '@/features/dashboard/types'
import { formatCurrency } from '@/shared/formatting/formatCurrency'
import { formatDate, formatDateTime } from '@/shared/formatting/formatDate'
import { formatNumber, formatVolume } from '@/shared/formatting/formatNumber'
import { formatPercentage } from '@/shared/formatting/formatPercentage'

function toDisplayName(symbol: string) {
  if (symbol.endsWith('USDT')) {
    return `${symbol.slice(0, -4)} / USDT`
  }

  return symbol
}

export function toDatasetSummaryForOverview(
  overview: DashboardOverviewDto | null,
): DatasetSummaryForOverview | null {
  if (!overview) {
    return null
  }

  const { symbol, interval, candleCount, totalVolume, firstCandleTime, lastCandleTime } =
    overview
  const lastCandleAt = lastCandleTime

  let rangeLabel = 'No candles in selected period'
  if (firstCandleTime && lastCandleTime) {
    rangeLabel = `${formatDate(firstCandleTime)} – ${formatDate(lastCandleTime)}`
  }

  return {
    symbol,
    displayName: toDisplayName(symbol),
    interval,
    rangeLabel,
    candleCount,
    formattedCandleCount: formatNumber(candleCount),
    lastCandleAt,
    formattedLastCandleAt: lastCandleAt
      ? formatDateTime(lastCandleAt)
      : 'Unavailable',
    formattedVolume: formatVolume(totalVolume),
  }
}

export function buildStatisticsForOverview(
  overview: DashboardOverviewDto | null,
): StatisticForOverview[] {
  if (!overview) {
    return []
  }

  const changePercent = overview.changePercent
  const changeTone: StatisticForOverview['tone'] =
    changePercent === null
      ? 'neutral'
      : changePercent >= 0
        ? 'success'
        : 'danger'

  const athDistancePercent = overview.athDistancePercent
  const athTone: StatisticForOverview['tone'] =
    athDistancePercent >= 0 ? 'success' : 'danger'

  return [
    {
      id: 'latest-close',
      label: 'Current close',
      value: formatCurrency(overview.latestClose),
    },
    {
      id: 'price-change',
      label: 'Price change',
      value: changePercent === null ? '—' : formatPercentage(changePercent),
      tone: changeTone,
    },
    {
      id: 'highest-high',
      label: 'Highest price',
      value: formatCurrency(overview.highestHigh),
    },
    {
      id: 'lowest-low',
      label: 'Lowest price',
      value: formatCurrency(overview.lowestLow),
    },
    {
      id: 'ath-distance',
      label: 'ATH Distance',
      value: formatPercentage(athDistancePercent),
      tone: athTone,
    },
    {
      id: 'volume',
      label: 'Trading volume',
      value: formatVolume(overview.totalVolume),
    },
  ]
}

export function toDashboardOverviewModel(
  overview: DashboardOverviewDto | null,
): DashboardOverviewModel {
  return {
    statistics: buildStatisticsForOverview(overview),
    dataset: toDatasetSummaryForOverview(overview),
  }
}
