import { inject, Injectable } from '@angular/core';
import { ApiService } from '../utils/api.service';
import { StatDto } from '../dtos/stat.dto';
import { Observable } from 'rxjs';
import { CreateEditStatDto } from '../dtos/parameters/create-edit-stat.dto';

@Injectable({ providedIn: 'root' })
export class StatService {
  private apiService = inject(ApiService);

  getExerciseStats(exerciseId: number): Observable<StatDto[]> {
    return this.apiService.get<StatDto[]>(`stats/exercises/${exerciseId}`);
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
