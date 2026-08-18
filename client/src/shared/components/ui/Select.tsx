import * as RadixSelect from '@radix-ui/react-select'
import { Check, ChevronDown } from 'lucide-react'
import type { ChangeEvent, ChangeEventHandler } from 'react'
import { cn } from '@/lib/cn'

type SelectOption = {
  value: string
  label: string
}

type SelectProps = {
  label?: string
  options: SelectOption[]
  value?: string
  defaultValue?: string
  onChange?: ChangeEventHandler<HTMLSelectElement>
  disabled?: boolean
  name?: string
  required?: boolean
  className?: string
  id?: string
}

function synthesizeChangeEvent(value: string): ChangeEvent<HTMLSelectElement> {
  return {
    target: { value },
    currentTarget: { value },
  } as ChangeEvent<HTMLSelectElement>
}

export default function Select({
  label,
  options,
  className,
  id,
  value,
  defaultValue,
  onChange,
  disabled,
  name,
  required,
}: SelectProps) {
  const selectId = id ?? label?.toLowerCase().replace(/\s+/g, '-')

  return (
    <div className="flex flex-col gap-1 text-left">
      {label && (
        <label htmlFor={selectId} className="text-sm text-[var(--text)]">
          {label}
        </label>
      )}
      <RadixSelect.Root
        value={value}
        defaultValue={defaultValue}
        disabled={disabled}
        name={name}
        required={required}
        onValueChange={(nextValue) => {
          onChange?.(synthesizeChangeEvent(nextValue))
        }}
      >
        <RadixSelect.Trigger
          id={selectId}
          aria-label={label}
          className={cn(
            'inline-flex h-9 w-full items-center justify-between gap-2 rounded-lg border border-[var(--border)] bg-[var(--bg)] px-3 text-sm text-[var(--text-h)] outline-none transition-colors',
            'hover:border-[color-mix(in_srgb,var(--text)_28%,var(--border))]',
            'focus-visible:border-[var(--accent-border)] focus-visible:ring-2 focus-visible:ring-slate-400/30 dark:focus-visible:ring-slate-500/35',
            'disabled:cursor-not-allowed disabled:opacity-50',
            'data-[placeholder]:text-[var(--text)]',
            className,
          )}
        >
          <RadixSelect.Value placeholder="Select…" />
          <RadixSelect.Icon asChild>
            <ChevronDown className="h-3.5 w-3.5 shrink-0 text-[var(--text)]" />
          </RadixSelect.Icon>
        </RadixSelect.Trigger>

        <RadixSelect.Portal>
          <RadixSelect.Content
            position="popper"
            sideOffset={4}
            className="z-50 max-h-72 min-w-[var(--radix-select-trigger-width)] overflow-hidden rounded-lg border border-[var(--border)] bg-[var(--bg)] shadow-[var(--shadow)]"
          >
            <RadixSelect.Viewport className="p-1">
              {options.map((option) => (
                <RadixSelect.Item
                  key={option.value}
                  value={option.value}
                  className="relative flex cursor-pointer items-center rounded-md py-2 pr-8 pl-3 text-sm text-[var(--text-h)] outline-none select-none data-[disabled]:pointer-events-none data-[disabled]:opacity-50 data-[highlighted]:bg-slate-100 dark:data-[highlighted]:bg-slate-800"
                >
                  <RadixSelect.ItemText>{option.label}</RadixSelect.ItemText>
                  <RadixSelect.ItemIndicator className="absolute right-2 inline-flex items-center">
                    <Check className="h-3.5 w-3.5 text-[var(--text)]" />
                  </RadixSelect.ItemIndicator>
                </RadixSelect.Item>
              ))}
            </RadixSelect.Viewport>
          </RadixSelect.Content>
        </RadixSelect.Portal>
      </RadixSelect.Root>
    </div>
  )
}
