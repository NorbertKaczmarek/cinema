import React from 'react';

import { Button, ButtonVariant } from 'Components/Button';
import { cn } from 'Utils/cn';

interface Props {
  cancelText?: string;
  deleteText?: string;
  editText?: string;
  submitText?: string;
  className?: string;
  isEdit: boolean;
  onCancel: () => void;
  onDelete?: () => void;
  onEdit?: () => void;
  onSubmit?: () => void;
}

export const ActionButtons = ({
  cancelText = 'Anuluj',
  deleteText = 'Usuń',
  editText = 'Edytuj',
  submitText = 'Zapisz',
  className,
  isEdit,
  onCancel,
  onDelete,
  onEdit,
  onSubmit,
}: Props) => (
  <div className={cn('flex', isEdit ? 'justify-between' : 'justify-end', className)}>
    {isEdit && (
      <div className="flex gap-3">
        {!!onDelete && (
          <Button variant={ButtonVariant.Danger} onClick={onDelete}>
            {deleteText}
          </Button>
        )}
        <Button variant={ButtonVariant.Outline} onClick={onCancel}>
          {cancelText}
        </Button>
      </div>
    )}
    {isEdit ? (
      <Button onClick={onSubmit}>{submitText}</Button>
    ) : (
      <Button onClick={onEdit}>{editText}</Button>
    )}
  </div>
);
