import { inject, Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { ExerciseDto } from '../dtos/exercise.dto';
import { CreateEditExerciseDto } from '../dtos/parameters/create-edit-exercise.dto';
import { CreateEditPlanDto } from '../dtos/parameters/create-edit-plan.dto';
import { PlanDto } from '../dtos/plan.dto';
import { ApiService } from '../utils/api.service';

@Injectable({ providedIn: 'root' })
export class WorkoutService {
  private apiService = inject(ApiService);

  private readonly plansKey = 'Plans';
  private readonly exercisesKey = 'Exercises';

  private _plans: PlanDto[] = JSON.parse(sessionStorage.getItem(this.plansKey) ?? '[]');
  get plans(): PlanDto[] {
    return this._plans;
  }
  private set plans(value: PlanDto[]) {
    sessionStorage.setItem(this.plansKey, JSON.stringify(value));
    this._plans = value;
  }

  private _exercises: ExerciseDto[] = JSON.parse(sessionStorage.getItem(this.exercisesKey) ?? '[]');
  get exercises(): ExerciseDto[] {
    return this._exercises;
  }
  private set exercises(value: ExerciseDto[]) {
    value = value?.sort((a, b) => (a.name.toLocaleLowerCase() > b.name.toLocaleLowerCase() ? 1 : -1));
    sessionStorage.setItem(this.exercisesKey, JSON.stringify(value));
    this._exercises = value;
  }

  async refreshPlans(): Promise<PlanDto[]> {
    const plans = await firstValueFrom(this.getPlans());
    this.plans = plans;

    return plans;
  }

  async refreshExercises(): Promise<ExerciseDto[]> {
    const exercises = await firstValueFrom(this.getExercises());
    this.exercises = exercises;

    return exercises;
  }

  async refreshPlansAndExercises(): Promise<[PlanDto[], ExerciseDto[]]> {
    return await Promise.all([await this.refreshPlans(), await this.refreshExercises()]);
  }

  private getPlans(): Observable<PlanDto[]> {
    return this.apiService.get<PlanDto[]>('plans');
  }

  createPlan(parameters: CreateEditPlanDto): Observable<number> {
    return this.apiService.post<number>('plans', parameters);
  }

  editPlan(planId: number, parameters: CreateEditPlanDto): Observable<number> {
    return this.apiService.patch<number>(`plans/${planId}`, parameters);
  }

  deletePlan(planId: number): Observable<number> {
    return this.apiService.delete<number>(`plans/${planId}`);
  }

  private getExercises(): Observable<ExerciseDto[]> {
    return this.apiService.get<ExerciseDto[]>('exercises');
  }

  createExercise(parameters: CreateEditExerciseDto): Observable<number> {
    return this.apiService.post<number>('exercises', parameters);
  }

  editExercise(exerciseId: number, parameters: CreateEditExerciseDto): Observable<number> {
    return this.apiService.patch<number>(`exercises/${exerciseId}`, parameters);
  }

  deleteExercise(exerciseId: number): Observable<number> {
    return this.apiService.delete<number>(`exercises/${exerciseId}`);
  }
}
