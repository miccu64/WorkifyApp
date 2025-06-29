import { BodyPartEnum } from './enums/body-part.enum';

export interface ExerciseDto {
  id: number;
  name: string;
  bodyPart: BodyPartEnum;
  description: string | null;
  isCustom: boolean;
}
