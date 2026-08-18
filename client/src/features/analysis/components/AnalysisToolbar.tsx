import {
  PERIOD_OPTIONS,
  TIMEFRAME_OPTIONS,
  type Period,
  type Timeframe,
} from '@/shared/constants/market'
import Select from '@/shared/components/ui/Select'
import Stack from '@/shared/components/ui/Stack'

type AnalysisToolbarProps = {
  timeframe: Timeframe
  period: Period
  onTimeframeChange: (timeframe: Timeframe) => void
  onPeriodChange: (period: Period) => void
}

export default function AnalysisToolbar({
  timeframe,
  period,
  onTimeframeChange,
  onPeriodChange,
}: AnalysisToolbarProps) {
  return (
    <Stack direction="row" gap={4} className="flex-wrap items-end">
      <Select
        label="Timeframe"
        options={TIMEFRAME_OPTIONS}
        value={timeframe}
        onChange={(event) =>
          onTimeframeChange(event.target.value as Timeframe)
        }
        className="min-w-[8rem]"
      />
      <Select
        label="Date range"
        options={PERIOD_OPTIONS}
        value={period}
        onChange={(event) => onPeriodChange(event.target.value as Period)}
        className="min-w-[10rem]"
      />
    </Stack>
  )
}
