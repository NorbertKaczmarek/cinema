import { Input, Props as SearchProps } from 'Components/Input';
import { cn } from 'Utils/cn';

export const DataTableSearch = ({ className, ...rest }: SearchProps) => (
  <Input className={cn('h-10 w-full lg:w-64', className)} {...rest} />
);
