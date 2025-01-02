import * as React from 'react';
import { FC } from 'react';

import { format } from 'date-fns';
import { CalendarIcon } from 'lucide-react';
import { FormProvider, UseFormReturn } from 'react-hook-form';

import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { Button, ButtonSize, ButtonVariant } from 'Components/Button';
import { Calendar } from 'Components/Calendar';
import { Card } from 'Components/Card';
import {
  FormControl,
  FormField as NewFormField,
  FormItem,
  FormLabel,
  FormMessage,
} from 'Components/Form/Form';
import { FormField } from 'Components/FormField';
import { Popover, PopoverContent, PopoverTrigger } from 'Components/Popover';
import { BaseScrollArea, BaseScrollBar } from 'Components/ScrollArea';
import { Select } from 'Components/Select';
import { Screening } from 'Types/screening';
import { cn } from 'Utils/cn';

interface Props {
  form: UseFormReturn<Screening>;
  isDisabled?: boolean;
}

export const ScreeningForm: FC<Props> = ({ form, isDisabled = false }) => {
  const { data } = useAdminMovies({ page: 0, size: 0 });

  const movieOptions = (data?.content || []).map(({ title, id }) => ({ label: title, value: id }));

  function handleDateSelect(date: Date | undefined) {
    if (date) {
      form.setValue('startDateTime', date.toISOString());
    }
  }

  function handleTimeChange(type: 'hour' | 'minute', value: string) {
    const currentDate = form.getValues('startDateTime') || new Date();
    const newDate = new Date(currentDate);

    if (type === 'hour') {
      const hour = parseInt(value, 10);
      newDate.setHours(hour);
    } else if (type === 'minute') {
      newDate.setMinutes(parseInt(value, 10));
    }

    form.setValue('startDateTime', newDate.toISOString());
  }

  return (
    <Card>
      <FormProvider {...form}>
        <form className="grid grid-cols-1 gap-6">
          <NewFormField
            control={form.control}
            name="startDateTime"
            render={({ field }) => (
              <FormItem className="flex flex-col">
                <FormLabel>Data rozpoczęcia seansu</FormLabel>
                <Popover>
                  <PopoverTrigger asChild>
                    <FormControl>
                      <Button
                        variant={ButtonVariant.Outline}
                        className={cn(
                          'w-full pl-3 text-left font-normal',
                          !field.value && 'text-muted-foreground'
                        )}
                      >
                        {field.value ? (
                          format(field.value, 'MM/dd/yyyy HH:mm')
                        ) : (
                          <span>MM/DD/YYYY HH:mm</span>
                        )}
                        <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                      </Button>
                    </FormControl>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0">
                    <div className="sm:flex">
                      <Calendar
                        mode="single"
                        selected={new Date(field.value)}
                        onSelect={handleDateSelect}
                        initialFocus
                      />
                      <div className="flex flex-col divide-y sm:h-[300px] sm:flex-row sm:divide-x sm:divide-y-0">
                        <BaseScrollArea className="w-64 sm:w-auto">
                          <div className="flex p-2 sm:flex-col">
                            {Array.from({ length: 24 }, (_, i) => i)
                              .reverse()
                              .map((hour, i) => (
                                <Button
                                  key={`${hour} - ${i} - asdf`}
                                  size={ButtonSize.Icon}
                                  variant={
                                    field.value && new Date(field.value).getHours() === hour
                                      ? ButtonVariant.Primary
                                      : ButtonVariant.Ghost
                                  }
                                  className="aspect-square shrink-0 sm:w-full"
                                  onClick={() => handleTimeChange('hour', hour.toString())}
                                >
                                  {hour}
                                </Button>
                              ))}
                          </div>
                          <BaseScrollBar orientation="horizontal" className="sm:hidden" />
                        </BaseScrollArea>
                        <BaseScrollArea className="w-64 sm:w-auto">
                          <div className="flex p-2 sm:flex-col">
                            {Array.from({ length: 12 }, (_, i) => i * 5).map((minute, i) => (
                              <Button
                                key={`${minute} - ${i} - fdsa`}
                                size={ButtonSize.Icon}
                                variant={
                                  field.value && new Date(field.value).getHours() === minute
                                    ? ButtonVariant.Primary
                                    : ButtonVariant.Ghost
                                }
                                className="aspect-square shrink-0 sm:w-full"
                                onClick={() => handleTimeChange('minute', minute.toString())}
                              >
                                {minute.toString().padStart(2, '0')}
                              </Button>
                            ))}
                          </div>
                          <BaseScrollBar orientation="horizontal" className="sm:hidden" />
                        </BaseScrollArea>
                      </div>
                    </div>
                  </PopoverContent>
                </Popover>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField name="movieId" label="Film" control={form.control}>
            <Select options={movieOptions} disabled={isDisabled} />
          </FormField>
        </form>
      </FormProvider>
    </Card>
  );
};
