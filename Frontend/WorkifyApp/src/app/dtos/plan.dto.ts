export interface PlanDto {
  id: number;
  name: string;
  description: string | null;
  exercisesIds: number[];
}
