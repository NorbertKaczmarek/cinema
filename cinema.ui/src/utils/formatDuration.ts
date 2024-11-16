export const formatDuration = (minutes: number): string => {
  if (minutes <= 0) return '0 min';

  const hours = Math.floor(minutes / 60);
  const remainingMinutes = minutes % 60;

  return `${hours ? `${hours}h` : ''} ${remainingMinutes}min`;
};
