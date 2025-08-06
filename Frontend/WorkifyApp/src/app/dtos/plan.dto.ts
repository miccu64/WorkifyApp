export type PlanDto = {
  id: number;
  name: string;
  description: string | null;
  exercisesIds: number[];
};
