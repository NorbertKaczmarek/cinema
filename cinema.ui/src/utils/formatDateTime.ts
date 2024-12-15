export const formatDateTime = (v: string): { date: string; time: string } => {
  const newV = v.split('T');

  return {
    date: newV[0],
    time: newV[1].slice(0, 5),
  };
};
