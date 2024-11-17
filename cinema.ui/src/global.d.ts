export {};

declare global {
  export type ClassNames<T extends string> = {
    [K in T]?: string;
  };

  export type Nullable<T> = T | null;
}
