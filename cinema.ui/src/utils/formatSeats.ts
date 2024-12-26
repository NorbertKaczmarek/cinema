import { Seat } from 'Types/screening';

export const formatSeats = (ids: string[], seats: Seat[]): string =>
  ids
    .map(id => {
      const fullSeat = seats.find(seat => seat.id === id);

      return `${fullSeat?.row}${fullSeat?.number}`;
    })
    .join(', ');
