import { useAdminScreenings } from 'Api/queries/useAdminScreenings';
import { formatDateTime } from 'Utils/formatDateTime';

interface Props {
  screeningId: string;
}

export const ScreeningDateCell = ({ screeningId }: Props) => {
  const { data: screenings } = useAdminScreenings({ page: 0, size: 0 });

  const startDateTime = (screenings?.content || []).find(
    ({ id }) => id === screeningId
  )?.startDateTime;

  if (!startDateTime) return <div />;

  const { date, time } = formatDateTime(startDateTime || '');

  return <div>{`${date} ${time}`}</div>;
};
