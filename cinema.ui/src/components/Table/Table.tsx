import { forwardRef, HTMLAttributes, ReactNode, TdHTMLAttributes, ThHTMLAttributes } from 'react';

import { cn } from 'Utils/cn';

const BaseWrapper = forwardRef<HTMLTableElement, HTMLAttributes<HTMLTableElement>>(
  ({ className, ...props }, ref) => (
    <div className="relative w-full overflow-auto">
      <table ref={ref} className={cn('w-full caption-bottom text-sm', className)} {...props} />
    </div>
  )
);
BaseWrapper.displayName = 'TableWrapper';

const BaseHeader = forwardRef<HTMLTableSectionElement, HTMLAttributes<HTMLTableSectionElement>>(
  ({ className, ...props }, ref) => (
    <thead ref={ref} className={cn('[&_tr]:border-b', className)} {...props} />
  )
);
BaseHeader.displayName = 'TableHeader';

const BaseBody = forwardRef<HTMLTableSectionElement, HTMLAttributes<HTMLTableSectionElement>>(
  ({ className, ...props }, ref) => (
    <tbody ref={ref} className={cn('[&_tr:last-child]:border-0', className)} {...props} />
  )
);
BaseBody.displayName = 'TableBody';

const BaseFooter = forwardRef<HTMLTableSectionElement, HTMLAttributes<HTMLTableSectionElement>>(
  ({ className, ...props }, ref) => (
    <tfoot
      ref={ref}
      className={cn('border-t bg-muted/50 font-medium [&>tr]:last:border-b-0', className)}
      {...props}
    />
  )
);
BaseFooter.displayName = 'TableFooter';

const BaseRow = forwardRef<HTMLTableRowElement, HTMLAttributes<HTMLTableRowElement>>(
  ({ className, ...props }, ref) => (
    <tr
      ref={ref}
      className={cn(
        'border-b transition-colors hover:bg-muted/50 data-[state=selected]:bg-muted',
        className
      )}
      {...props}
    />
  )
);
BaseRow.displayName = 'TableRow';

const BaseHead = forwardRef<HTMLTableCellElement, ThHTMLAttributes<HTMLTableCellElement>>(
  ({ className, ...props }, ref) => (
    <th
      ref={ref}
      className={cn(
        'h-10 px-2 text-left align-middle font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]',
        className
      )}
      {...props}
    />
  )
);
BaseHead.displayName = 'TableHead';

const BaseCell = forwardRef<HTMLTableCellElement, TdHTMLAttributes<HTMLTableCellElement>>(
  ({ className, ...props }, ref) => (
    <td
      ref={ref}
      className={cn(
        'p-2 align-middle [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]',
        className
      )}
      {...props}
    />
  )
);
BaseCell.displayName = 'TableCell';

const BaseCaption = forwardRef<HTMLTableCaptionElement, HTMLAttributes<HTMLTableCaptionElement>>(
  ({ className, ...props }, ref) => (
    <caption ref={ref} className={cn('mt-4 text-sm text-muted-foreground', className)} {...props} />
  )
);
BaseCaption.displayName = 'TableCaption';

type ColumnItem = {
  index: string;
  className?: string;
  content: ReactNode;
};

export interface TableProps {
  caption?: ReactNode;
  classNames?: ClassNames<
    'wrapper' | 'caption' | 'header' | 'headerRow' | 'head' | 'body' | 'row' | 'cell' | 'footer'
  >;
  columns?: ColumnItem[];
  customColumns?: ReactNode;
  content?: Record<string, ReactNode>[];
  customContent?: ReactNode;
  footer?: ReactNode;
  footerContent?: ReactNode;
}

export const Table = ({
  caption,
  classNames,
  columns,
  customColumns,
  content,
  customContent,
  footer,
  footerContent,
}: TableProps) => (
  <BaseWrapper className={classNames?.wrapper}>
    {caption && <BaseCaption className={classNames?.caption}>{caption}</BaseCaption>}
    {customColumns}
    {!!columns && (
      <BaseHeader className={classNames?.header}>
        <BaseRow className={classNames?.headerRow}>
          {columns.map(({ className, content }, index) => (
            <BaseHead className={cn(classNames?.head, className)} key={`${index}-${className}`}>
              {content}
            </BaseHead>
          ))}
        </BaseRow>
      </BaseHeader>
    )}

    <BaseBody className={classNames?.body}>
      {customContent}
      {content?.map((element, index) => (
        <BaseRow key={`${element}-${index}`}>
          {columns?.map((column, colIndex) => (
            <BaseCell className={column.className} key={`${columns[colIndex].index}-${colIndex}`}>
              {element[column.index]}
            </BaseCell>
          ))}
        </BaseRow>
      ))}
    </BaseBody>

    {footerContent && <BaseFooter className={classNames?.footer}>{footerContent}</BaseFooter>}
    {footer}
  </BaseWrapper>
);

Table.Cell = BaseCell;
Table.Row = BaseRow;
Table.Head = BaseHead;
Table.Header = BaseHeader;

export default Table;
