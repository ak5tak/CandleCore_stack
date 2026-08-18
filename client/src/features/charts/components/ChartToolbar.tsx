import {
  TIMEFRAME_OPTIONS,
  type Timeframe,
} from '@/shared/constants/market'
import Select from '@/shared/components/ui/Select'

type ChartToolbarProps = {
  timeframe: Timeframe
  onTimeframeChange: (timeframe: Timeframe) => void
}

export default function ChartToolbar({
  timeframe,
  onTimeframeChange,
}: ChartToolbarProps) {
  return (
    <Select
      label="Interval"
      options={TIMEFRAME_OPTIONS}
      value={timeframe}
      onChange={(event) =>
        onTimeframeChange(event.target.value as Timeframe)
      }
      className="min-w-[8rem]"
    />
  )
}
