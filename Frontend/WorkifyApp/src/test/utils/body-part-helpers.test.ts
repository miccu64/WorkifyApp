import { BodyPartEnum } from '../../app/dtos/enums/body-part.enum';
import { getBodyParts } from '../../app/utils/body-part-helpers';

describe('getBodyParts', () => {
  it('Should return objects with bodyPart and name', () => {
    // Act
    const result = getBodyParts();

    // Assert
    expect(result.length).toBeGreaterThan(0);

    result.forEach(item => {
      expect(item).toHaveProperty('bodyPart');
      expect(item).toHaveProperty('name');
    });
  });

  it('Should include known enum values', () => {
    // Act
    const result = getBodyParts();

    // Assert
    const chest = result.find(r => r.bodyPart === BodyPartEnum.Chest);
    const back = result.find(r => r.bodyPart === BodyPartEnum.Back);

    expect(chest?.name).toBe('Chest');
    expect(back?.name).toBe('Back');
  });

  it('Should have unique bodyPart values', () => {
    // Act
    const result = getBodyParts();

    // Assert
    const bodyParts = result.map(r => r.bodyPart);
    const unique = new Set(bodyParts);
    expect(unique.size).toBe(bodyParts.length);
  });
});
