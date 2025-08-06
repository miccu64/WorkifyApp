import { BodyPartEnum } from './enums/body-part.enum';

export type ExerciseDto = {
  id: number;
  name: string;
  bodyPart: BodyPartEnum;
  description: string | null;
  isCustom: boolean;
};
