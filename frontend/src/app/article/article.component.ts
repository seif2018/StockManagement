import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ArticleService } from '../services/article.service';
import { SignalRService } from '../services/signalr.service';
import { EditArticleDialogComponent } from '../edit-article-dialog/edit-article-dialog.component';
import { MatTooltip, MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-article',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSnackBarModule,
    MatTableModule,
    MatIconModule,
    MatDialogModule,MatTooltip,
    EditArticleDialogComponent
  ],
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css']
})
export class ArticleComponent implements OnInit {
  articles: any[] = [];
  displayedColumns: string[] = ['reference', 'nom', 'prixTTC', 'quantiteStock', 'type', 'actions'];

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  articleForm: FormGroup;
  approForm: FormGroup;
  inventaireForm: FormGroup;

  constructor(
    private articleService: ArticleService,
    private signalRService: SignalRService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private fb: FormBuilder
  ) {
    const today = new Date().toISOString().split('T')[0];

    this.articleForm = this.fb.group({
      reference: ['', [Validators.required, Validators.pattern('^[0-9]{13}$')]],
      nom: ['', Validators.required],
      prixHT: [0, [Validators.required, Validators.min(0.01)]],
      typeArticle: ['Alimentaire', Validators.required],
      dlc: [today, Validators.required],
      typeVente: ['LesDeux'],
      packaging: ['Neuf']
    });

    this.approForm = this.fb.group({
      reference: ['', [Validators.required, Validators.pattern('^[0-9]{13}$')]],
      quantite: [0, [Validators.required, Validators.min(0)]]
    });

    this.inventaireForm = this.fb.group({
      reference: ['', [Validators.required, Validators.pattern('^[0-9]{13}$')]],
      nouvelleQuantite: [0, [Validators.required, Validators.min(0)]]
    });

    // Réagir au changement de type d'article pour la DLC
    this.articleForm.get('typeArticle')?.valueChanges.subscribe(type => {
      const dlcControl = this.articleForm.get('dlc');
      if (type === 'Alimentaire') {
        dlcControl?.setValidators([Validators.required]);
        if (!dlcControl?.value) {
          dlcControl?.setValue(new Date().toISOString().split('T')[0]);
        }
      } else {
        dlcControl?.clearValidators();
        dlcControl?.setValue(null);
      }
      dlcControl?.updateValueAndValidity();
    });
  }

  ngOnInit(): void {
    this.loadArticlesPaged();
    this.signalRService.startConnection();
    this.signalRService.addStockUpdatedListener((data: any) => {
      this.snackBar.open(`Stock mis à jour: ${data.reference} -> ${data.nouveauStock}`, 'Fermer', { duration: 3000 });
      this.loadArticlesPaged();
    });
  }

  loadArticlesPaged(): void {
    this.articleService.getArticlesPaged(this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.articles = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
      },
      error: () => this.snackBar.open('Erreur chargement articles', 'Fermer', { duration: 3000 })
    });
  }

  private resetForm(formGroup: FormGroup, defaultValues: any): void {
  formGroup.reset(defaultValues);
  Object.keys(formGroup.controls).forEach(key => {
    const control = formGroup.get(key);
    control?.markAsPristine();
    control?.markAsUntouched();
    control?.setErrors(null);
    control?.updateValueAndValidity();
  });
}

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadArticlesPaged();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadArticlesPaged();
    }
  }

  onCreateArticle(): void {
      this.articleService.createArticle(this.articleForm.value).subscribe({
    next: () => {
      this.snackBar.open('Article créé', 'OK', { duration: 2000 });
      const today = new Date().toISOString().split('T')[0];
      this.resetForm(this.articleForm, {
        reference: '',
        nom: '',
        prixHT: 0,
        typeArticle: 'Alimentaire',
        dlc: today,
        typeVente: 'LesDeux',
        packaging: 'Neuf'
      });      
        this.loadArticlesPaged();
      },
      error: (err) => {
        this.snackBar.open(err.message, 'Fermer', { duration: 4000 });
      }
    });
  }

  onApprovisionner(): void {
    this.articleService.approvisionner(this.approForm.value).subscribe({
      next: () => {
        this.snackBar.open('Approvisionnement effectué', 'Fermer', { duration: 2000 });
        this.resetForm(this.approForm, { reference: '', quantite: 0 });
        this.loadArticlesPaged();
      },
      error: (err) => {
        this.snackBar.open(err.message || 'Erreur lors de l\'approvisionnement', 'Fermer', { duration: 4000 });
      }
    });
  }

  onInventaire(): void {
    this.articleService.inventaire(this.inventaireForm.value).subscribe({
      next: () => {
        this.snackBar.open('Inventaire mis à jour', 'Fermer', { duration: 2000 });
        this.inventaireForm.reset();
        this.inventaireForm.markAsPristine();
        this.inventaireForm.markAsUntouched();
        this.loadArticlesPaged();
      },
      error: (err) => {
        this.snackBar.open(err.message || 'Erreur lors de l\'inventaire', 'Fermer', { duration: 4000 });
      }
    });
  }

  onEdit(article: any): void {
    const dialogRef = this.dialog.open(EditArticleDialogComponent, {
      width: '500px',
      data: { article }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadArticlesPaged();
        this.snackBar.open('Article modifié', 'OK', { duration: 2000 });
      }
    });
  }

  onDelete(reference: string): void {
    const confirmDelete = confirm(`Supprimer définitivement l'article ${reference} ?`);
    if (confirmDelete) {
      this.articleService.deleteArticle(reference).subscribe({
        next: () => {
          this.snackBar.open('Article supprimé', 'OK', { duration: 2000 });
          this.loadArticlesPaged();
        },
        error: (err) => {
          this.snackBar.open(err.error?.error || err.message || 'Erreur lors de la suppression', 'Fermer', { duration: 4000 });
        }
      });
    }
  }
}