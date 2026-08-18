export const TIMEFRAMES = ['1h', '4h', '1d', '1w', '1M'] as const
export const PERIODS = ['7d', '30d', '90d', '1y', 'all'] as const

export type Timeframe = (typeof TIMEFRAMES)[number]
export type Period = (typeof PERIODS)[number]

const TIMEFRAME_LABELS = {
  '1h': '1H',
  '4h': '4H',
  '1d': '1D',
  '1w': '1W',
  '1M': '1M',
} as const satisfies Record<Timeframe, string>

export const TIMEFRAME_OPTIONS: { value: Timeframe; label: string }[] =
  TIMEFRAMES.map((value) => ({
    value,
    label: TIMEFRAME_LABELS[value],
  }))

const PERIOD_LABELS = {
  '7d': 'Last 7 days',
  '30d': 'Last 30 days',
  '90d': 'Last 90 days',
  '1y': 'Last year',
  all: 'All',
} as const satisfies Record<Period, string>

export const PERIOD_OPTIONS: { value: Period; label: string }[] =
  PERIODS.map((value) => ({
    value,
    label: PERIOD_LABELS[value],
  }))

export function parseTimeframe(
  value: unknown,
  fallback: Timeframe,
): Timeframe {
  if (
    typeof value === 'string' &&
    (TIMEFRAMES as readonly string[]).includes(value)
  ) {
    return value as Timeframe
  }

  return fallback
}

export function parsePeriod(value: unknown, fallback: Period): Period {
  if (
    typeof value === 'string' &&
    (PERIODS as readonly string[]).includes(value)
  ) {
    return value as Period
  }

  return fallback
}

export function getPeriodDateRange(
  period: Period,
  anchorEnd: Date,
  firstOpenTime?: Date,
): {
  startDate: string
  endDate: string
} {
  const end = new Date(anchorEnd)
  const start = new Date(end)

  switch (period) {
    case '7d':
      start.setUTCDate(start.getUTCDate() - 7)
      break
    case '30d':
      start.setUTCDate(start.getUTCDate() - 30)
      break
    case '90d':
      start.setUTCDate(start.getUTCDate() - 90)
      break
    case '1y':
      start.setUTCFullYear(start.getUTCFullYear() - 1)
      break
    case 'all':
      if (firstOpenTime) {
        start.setTime(firstOpenTime.getTime())
      }
      break
  }

  if (period !== 'all' && firstOpenTime && start < firstOpenTime) {
    start.setTime(firstOpenTime.getTime())
  }

  return {
    startDate: start.toISOString(),
    endDate: end.toISOString(),
  }
}
