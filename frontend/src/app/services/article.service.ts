import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ArticleService {
  private apiUrl = 'https://localhost:5001/api'; 

  constructor(private http: HttpClient) { }

  getArticles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/articles`);
  }

  createArticle(article: any): Observable<any> {
  return this.http.post(`${this.apiUrl}/articles`, article).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 400 && error.error?.error?.includes('existe déjà')) {
        // Ne pas propager l'erreur pour éviter l'affichage console
        return throwError(() => new Error(error.error.error));
      }
      return throwError(() => error);
    })
  );
}
updateArticle(reference: string, article: any): Observable<any> {
  return this.http.put(`${this.apiUrl}/articles/${reference}`, article);
}

deleteArticle(reference: string): Observable<any> {
  return this.http.delete(`${this.apiUrl}/articles/${reference}`);
}

getArticlesPaged(page: number, pageSize: number): Observable<any> {
  return this.http.get(`${this.apiUrl}/articles/paged?page=${page}&pageSize=${pageSize}`);
}

  approvisionner(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/articles/approvisionner`, data);
  }

  inventaire(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/articles/inventaire`, data);
  }

   getMouvements(): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/mouvements`);
}

exportInventairesCSV(): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/inventaires/export`);
}

getMouvementsPaged(page: number, pageSize: number, sortBy: string, descending: boolean, search: string = ''): Observable<any> {
  let url = `${this.apiUrl}/mouvements/paged?page=${page}&pageSize=${pageSize}&sortBy=${sortBy}&descending=${descending}`;
  if (search) {
    url += `&search=${encodeURIComponent(search)}`;
  }
  return this.http.get(url);
}

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Une erreur est survenue';
    if (error.error && error.error.error) {
      errorMessage = error.error.error;
    } else if (error.status === 400) {
      errorMessage = 'Requête invalide';
    }
    return throwError(() => new Error(errorMessage));
  }
}