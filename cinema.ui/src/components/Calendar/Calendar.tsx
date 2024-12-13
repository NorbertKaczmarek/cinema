import { pl } from 'date-fns/locale';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { Chevron, CustomComponents, DayPicker } from 'react-day-picker';
import { DayPickerProps } from 'react-day-picker/src/types/props';

import { baseButtonVariants } from 'Components/Button';
import { cn } from 'Utils/cn';

type NavIconProps = CustomComponents['Chevron'] & {
  className?: string;
  orientation?: 'left' | 'right';
};

const navIcon = ({ className, orientation, ...props }: NavIconProps) => {
  if (orientation === 'left')
    return <ChevronLeft className={cn('h-4 w-4', className)} {...props} />;
  if (orientation === 'right')
    return <ChevronRight className={cn('h-4 w-4', className)} {...props} />;
  return <Chevron {...props} />;
};

export type BaseCalendarProps = DayPickerProps;

export const BaseCalendar = ({
  className,
  classNames,
  showOutsideDays = true,
  ...props
}: BaseCalendarProps) => (
  <DayPicker
    showOutsideDays={showOutsideDays}
    ISOWeek
    className={cn('relative p-3', className)}
    classNames={{
      months: 'flex flex-col sm:flex-row',
      month: 'space-y-4 [&:not(:first-of-type)]:ml-4',
      month_caption: 'flex justify-center pt-1 relative items-center',
      caption_label: 'text-sm font-medium',
      nav: 'space-x-1 flex items-start z-10',
      button_previous: cn(
        baseButtonVariants({ variant: 'outline' }),
        'h-7 w-7 bg-transparent p-0 opacity-50 hover:opacity-100',
        'absolute left-4'
      ),
      button_next: cn(
        baseButtonVariants({ variant: 'outline' }),
        'h-7 w-7 bg-transparent p-0 opacity-50 hover:opacity-100',
        'absolute right-4'
      ),
      month_grid: 'w-full border-collapse space-y-1',
      weekdays: 'flex',
      weekday: 'text-muted-foreground rounded-md w-9 font-normal text-[0.8rem]',
      week: 'flex w-full mt-2',
      day: 'h-9 w-9 text-center text-sm p-0 relative [&:has([aria-selected].day-range-end)]:rounded-r-md [&:has([aria-selected].day-outside)]:bg-accent/50 [&:has([aria-selected])]:bg-accent first:[&:has([aria-selected])]:rounded-l-md last:[&:has([aria-selected])]:rounded-r-md focus-within:relative focus-within:z-20',
      day_button: cn(
        baseButtonVariants({ variant: 'ghost' }),
        'h-9 w-9 p-0 font-normal aria-selected:opacity-100 hover:bg-primary hover:text-primary-foreground '
      ),
      range_start: 'day-range-start',
      range_middle: 'aria-selected:bg-accent aria-selected:text-accent-foreground rounded-none',
      range_end: 'day-range-end',
      selected:
        'bg-primary text-primary-foreground rounded-md hover:bg-primary hover:text-primary-foreground focus:bg-primary focus:text-primary-foreground [&.day-range-end]:rounded-r-md [&.day-range-start]:rounded-l-md',
      outside:
        'day-outside text-muted-foreground opacity-50 aria-selected:bg-accent/50 aria-selected:text-muted-foreground aria-selected:opacity-30',
      disabled: 'text-muted-foreground opacity-50',
      hidden: 'invisible',
      ...classNames,
    }}
    components={{
      Chevron: navIcon as CustomComponents['Chevron'],
    }}
    {...props}
  />
);

BaseCalendar.displayName = 'Calendar';

export enum CalendarMode {
  Single = 'single',
  Range = 'range',
  Multiple = 'multiple',
}

export type CalendarProps = {
  className?: string;
} & BaseCalendarProps;

export const Calendar = ({ className, ...props }: CalendarProps) => (
  <BaseCalendar locale={pl} className={cn('rounded-md border', className)} {...props} />
);
