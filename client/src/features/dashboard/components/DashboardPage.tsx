import DashboardOverview from '@/features/dashboard/components/DashboardOverview'
import type {
  DatasetSummaryForOverview,
  StatisticForOverview,
} from '@/features/dashboard/types'
import type { Period } from '@/shared/constants/market'

type DashboardPageProps = {
  period: Period
  onPeriodChange: (period: Period) => void
  statistics: StatisticForOverview[]
  dataset: DatasetSummaryForOverview | null
}

export default function DashboardPage({
  period,
  onPeriodChange,
  statistics,
  dataset,
}: DashboardPageProps) {
  return (
    <DashboardOverview
      period={period}
      onPeriodChange={onPeriodChange}
      statistics={statistics}
      dataset={dataset}
    />
  )
}
