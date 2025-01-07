export enum OrderStatus {
  READY = 'Ready',
  PENDING = 'Pending',
  CANCELLED = 'Cancelled',
}

export type Order = {
  id: string;
  email: string;
  phoneNumber: string;
  status: OrderStatus;
  screeningId: string;
  seatIds: string[];
  fourDigitCode: string;
};
