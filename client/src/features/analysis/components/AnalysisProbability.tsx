import type { ProbabilityBucketForAnalysis } from '@/features/analysis/types'
import Badge from '@/shared/components/ui/Badge'
import Card from '@/shared/components/ui/Card'
import EmptyState from '@/shared/components/ui/EmptyState'
import Grid from '@/shared/components/ui/Grid'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'

type AnalysisProbabilityProps = {
  buckets: ProbabilityBucketForAnalysis[]
}

function ProbabilityCard({ bucket }: { bucket: ProbabilityBucketForAnalysis }) {
  return (
    <Card>
      <Stack gap={2}>
        <Text size="sm" muted>
          {bucket.label}
        </Text>
        <Badge tone={bucket.tone} className="w-fit text-base">
          {bucket.formattedPercent}
        </Badge>
        <Text size="sm" muted>
          {bucket.formattedCount} candles
        </Text>
      </Stack>
    </Card>
  )
}

export default function AnalysisProbability({
  buckets,
}: AnalysisProbabilityProps) {
  return (
    <Stack gap={4}>
      <Heading as={3}>Probability analysis</Heading>

      {buckets.length > 0 ? (
        <Grid cols={3}>
          {buckets.map((bucket) => (
            <ProbabilityCard key={bucket.id} bucket={bucket} />
          ))}
        </Grid>
      ) : (
        <Card padding="none">
          <EmptyState
            title="No probability data"
            description="Bullish, bearish, and neutral frequencies will appear once candles are available."
          />
        </Card>
      )}
    </Stack>
  )
}
