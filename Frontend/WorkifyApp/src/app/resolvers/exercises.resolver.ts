import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { WorkoutService } from '../services/workout.service';
import { ExerciseDto } from '../dtos/exercise.dto';

@Injectable({ providedIn: 'root' })
export class ExercisesResolver implements Resolve<Observable<ExerciseDto[]>> {
  constructor(private workoutService: WorkoutService) {}

  resolve(): Observable<ExerciseDto[]> {
    return this.workoutService.getExercises();
  }
}
