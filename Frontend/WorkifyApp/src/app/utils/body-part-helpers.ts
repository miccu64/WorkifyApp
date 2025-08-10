import { BodyPartEnum } from '../dtos/enums/body-part.enum';

export function getBodyParts(): { bodyPart: BodyPartEnum; name: string }[] {
  return Object.entries(BodyPartEnum)
    .filter(keyValue => Number.isInteger(keyValue[1]))
    .map(kv => {
      return { bodyPart: kv[1] as BodyPartEnum, name: kv[0] };
    });
}
