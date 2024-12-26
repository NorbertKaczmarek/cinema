import { create } from 'zustand';

interface OrderState {
  screeningId?: string;
  chosenSeats: string[];

  setOrderData: (screeningId: string, chosenSeats: string[]) => void;
  clearOrder: () => void;
}

const useOrderStore = create<OrderState>(set => ({
  screeningId: undefined,
  chosenSeats: [],
  email: null,
  phoneNumber: null,

  setOrderData: (screeningId, chosenSeats) => set({ screeningId, chosenSeats }),

  clearOrder: () => set({ screeningId: undefined, chosenSeats: [] }),
}));

export default useOrderStore;
