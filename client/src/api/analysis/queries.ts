import type { ApiClient } from '@/api/httpClient'
import {
  DEFAULT_LOOKBACK,
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import { getAnalysisOverview } from './client'
import type { OverviewParams } from './types'

export function analysisOverviewQueryOptions(
  apiClient: ApiClient,
  params: OverviewParams = {
    symbol: DEFAULT_SYMBOL,
    timeframe: DEFAULT_TIMEFRAME,
    lookback: DEFAULT_LOOKBACK,
  },
) {
  return {
    queryKey: ['analysis', 'overview', params] as const,
    queryFn: () => getAnalysisOverview(apiClient, params),
  }
}
