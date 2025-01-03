import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

interface OrderState {
  screeningId?: string;
  chosenSeats: string[];
  setOrderData: (screeningId: string, chosenSeats: string[]) => void;
  clearOrder: () => void;
}

const useOrderStore = create<OrderState>()(
  devtools(
    set => ({
      screeningId: undefined,
      chosenSeats: [],

      setOrderData: (screeningId, chosenSeats) => set({ screeningId, chosenSeats }),

      clearOrder: () => set({ screeningId: undefined, chosenSeats: [] }),
    }),
    { name: 'OrderStore' }
  )
);

export default useOrderStore;
