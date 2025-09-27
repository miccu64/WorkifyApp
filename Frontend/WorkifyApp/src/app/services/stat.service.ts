import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { CreateEditStatDto } from '../dtos/parameters/create-edit-stat.dto';
import { StatDto } from '../dtos/stat.dto';
import { ApiService } from '../utils/api.service';

@Injectable({ providedIn: 'root' })
export class StatService {
  private apiService = inject(ApiService);

  private cachedStats: Record<number, StatDto[]> = {};

  tryGetCachedStats(exerciseId: number): StatDto[] | null {
    return this.cachedStats[exerciseId];
  }

  cacheStats(exerciseId: number, stats: StatDto[]): void {
    this.cachedStats[exerciseId] = stats;
  }

  getExerciseStats(exerciseId: number): Observable<StatDto[]> {
    return this.apiService.get<StatDto[]>(`stats/exercises/${exerciseId}`).pipe(
      tap(stats => {
        this.cacheStats(exerciseId, stats);
      })
    );
  }

  createStat(exerciseId: number, dto: CreateEditStatDto): Observable<number> {
    return this.apiService.post<number>(`stats/exercises/${exerciseId}`, dto);
  }

  editStat(statId: number, dto: CreateEditStatDto): Observable<number> {
    return this.apiService.patch<number>(`stats/${statId}`, dto);
  }

  deleteStat(statId: number): Observable<number> {
    return this.apiService.delete<number>(`stats/${statId}`);
  }
}
