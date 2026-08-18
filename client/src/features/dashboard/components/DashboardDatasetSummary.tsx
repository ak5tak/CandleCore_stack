import type { DatasetSummaryForOverview } from '@/features/dashboard/types'
import Badge from '@/shared/components/ui/Badge'
import Card from '@/shared/components/ui/Card'
import EmptyState from '@/shared/components/ui/EmptyState'
import Grid from '@/shared/components/ui/Grid'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'

type DashboardDatasetSummaryProps = {
  dataset: DatasetSummaryForOverview | null
}

type MetaProps = {
  label: string
  value: string
}

function Meta({ label, value }: MetaProps) {
  return (
    <Stack gap={1}>
      <Text size="sm" muted>
        {label}
      </Text>
      <Heading as={4}>{value}</Heading>
    </Stack>
  )
}

export default function DashboardDatasetSummary({
  dataset,
}: DashboardDatasetSummaryProps) {
  return (
    <Card className="h-full">
      <Stack gap={4}>
        <Stack direction="row" gap={2} className="flex-wrap items-center">
          {dataset ? (
            <>
              <Heading as={3}>{dataset.displayName}</Heading>
              <Badge tone="accent">{dataset.interval}</Badge>
            </>
          ) : (
            <Heading as={3}>Historical dataset</Heading>
          )}
        </Stack>

        {dataset ? (
          <Grid cols={2}>
            <Meta label="Available range" value={dataset.rangeLabel} />
            <Meta label="Total candles" value={dataset.formattedCandleCount} />
            <Meta label="Last candle" value={dataset.formattedLastCandleAt} />
            <Meta label="Period volume" value={dataset.formattedVolume} />
          </Grid>
        ) : (
          <EmptyState
            title="No market data"
            description="Dataset details will appear here once candle data is available."
          />
        )}
      </Stack>
    </Card>
  )
}
