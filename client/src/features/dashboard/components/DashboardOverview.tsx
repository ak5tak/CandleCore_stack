import type {
  DatasetSummaryForOverview,
  StatisticForOverview,
} from '@/features/dashboard/types'
import Heading from '@/shared/components/ui/Heading'
import Select from '@/shared/components/ui/Select'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'
import { PERIOD_OPTIONS, type Period } from '@/shared/constants/market'
import DashboardDatasetSummary from './DashboardDatasetSummary'
import DashboardQuickStatistics from './DashboardQuickStatistics'

type DashboardOverviewProps = {
  period: Period
  onPeriodChange: (period: Period) => void
  statistics: StatisticForOverview[]
  dataset: DatasetSummaryForOverview | null
}

export default function DashboardOverview({
  period,
  onPeriodChange,
  statistics,
  dataset,
}: DashboardOverviewProps) {
  return (
    <Stack gap={8}>
      <Stack
        direction="row"
        gap={4}
        className="flex-wrap items-end justify-between"
      >
        <Stack gap={2}>
          <Heading as={1}>Dashboard</Heading>
          <Text muted>
            Current state of the Bitcoin dataset for the selected period.
          </Text>
        </Stack>

        <Select
          label="Period"
          options={PERIOD_OPTIONS}
          value={period}
          onChange={(event) => onPeriodChange(event.target.value as Period)}
          className="min-w-[10rem]"
        />
      </Stack>

      <DashboardQuickStatistics statistics={statistics} />

      <DashboardDatasetSummary dataset={dataset} />

      
    </Stack>
  )
}
