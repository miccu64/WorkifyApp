import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../utils/api.service';
import { PlanDto } from '../dtos/plan.dto';
import { ExerciseDto } from '../dtos/exercise.dto';
import { CreateEditPlanDto } from '../dtos/parameters/create-edit-plan.dto';

@Injectable({ providedIn: 'root' })
export class WorkoutService {
  private apiService = inject(ApiService);

  private readonly plansKey = 'Plans';
  private readonly exercisesKey = 'Exercises';

  private _plans: PlanDto[] = JSON.parse(sessionStorage.getItem(this.plansKey) ?? '[]');
  public get plans(): PlanDto[] {
    return this._plans;
  }
  public set plans(value: PlanDto[]) {
    sessionStorage.setItem(this.plansKey, JSON.stringify(value));
    this._plans = value;
  }

  private _exercises: ExerciseDto[] = JSON.parse(sessionStorage.getItem(this.exercisesKey) ?? '[]');
  public get exercises(): ExerciseDto[] {
    return this._exercises;
  }
  public set exercises(value: ExerciseDto[]) {
    sessionStorage.setItem(this.exercisesKey, JSON.stringify(value));
    this._exercises = value;
  }

  getPlans(): Observable<PlanDto[]> {
    return this.apiService.get<PlanDto[]>('plans');
  }

  getExercises(): Observable<ExerciseDto[]> {
    return this.apiService.get<ExerciseDto[]>('exercises');
  }

  createPlan(parameters: CreateEditPlanDto): Observable<number> {
    return this.apiService.post<number>('plans', parameters);
  }
}
