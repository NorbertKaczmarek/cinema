export type Screening = {
  id: string;
  movieId: string;
  startDateTime: string;
  endDateTime: string;
};

export type Seat = {
  id: string;
  row: string;
  number: string;
  isTaken: boolean;
};

export type SeatAvailability = {
  totalSeats: number;
  takenSeats: number;
  availableSeats: number;
  seats: Seat[];
};
