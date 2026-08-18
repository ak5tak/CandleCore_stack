import type { ApiClient } from '@/api/httpClient'
import {
  DEFAULT_LOOKBACK,
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import { getDashboardOverview } from './client'
import type { DashboardOverviewParams } from './types'

export function dashboardOverviewQueryOptions(
  apiClient: ApiClient,
  params: DashboardOverviewParams = {
    symbol: DEFAULT_SYMBOL,
    timeframe: DEFAULT_TIMEFRAME,
    lookback: DEFAULT_LOOKBACK,
  },
) {
  return {
    queryKey: ['dashboard', 'overview', params] as const,
    queryFn: () => getDashboardOverview(apiClient, params),
  }
}
