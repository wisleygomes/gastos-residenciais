import { useEffect, useState, useCallback } from 'react';
import { peopleApi, transactionsApi, totalsApi } from './api/api';
import type {
  Person,
  Transaction,
  TotalsReport,
  CreatePersonInput,
  CreateTransactionInput,
} from './types';
import PersonForm from './components/PersonForm';
import PeopleList from './components/PeopleList';
import TransactionForm from './components/TransactionForm';
import TransactionsList from './components/TransactionsList';
import TotalsView from './components/TotalsView';

type Tab = 'people' | 'transactions' | 'totals';

/**
 * Componente raiz da aplicação. Controla a navegação entre as três telas
 * do sistema (Pessoas, Transações, Totais) e centraliza o carregamento
 * de dados vindos da API .NET.
 */
export default function App() {
  const [tab, setTab] = useState<Tab>('people');

  const [people, setPeople] = useState<Person[]>([]);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [totals, setTotals] = useState<TotalsReport | null>(null);

  const [loading, setLoading] = useState(true);
  const [globalError, setGlobalError] = useState<string | null>(null);

  /**
   * Recarrega todos os dados usados pela aplicação. Como o cadastro de
   * pessoas afeta as transações e os totais (e vice-versa), é mais simples
   * e seguro recarregar tudo após qualquer operação de escrita.
   */
  const reloadAll = useCallback(async () => {
    setGlobalError(null);
    try {
      const [peopleData, transactionsData, totalsData] = await Promise.all([
        peopleApi.list(),
        transactionsApi.list(),
        totalsApi.get(),
      ]);
      setPeople(peopleData);
      setTransactions(transactionsData);
      setTotals(totalsData);
    } catch (err) {
      setGlobalError(
        err instanceof Error
          ? err.message
          : 'Não foi possível conectar à API. Verifique se o back-end está em execução em http://localhost:5000.'
      );
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    reloadAll();
  }, [reloadAll]);

  async function handleCreatePerson(input: CreatePersonInput) {
    await peopleApi.create(input);
    await reloadAll();
  }

  async function handleDeletePerson(id: string) {
    await peopleApi.remove(id);
    await reloadAll();
  }

  async function handleCreateTransaction(input: CreateTransactionInput) {
    await transactionsApi.create(input);
    await reloadAll();
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>Controle de Gastos Residenciais</h1>
        <nav className="tabs">
          <button className={tab === 'people' ? 'active' : ''} onClick={() => setTab('people')}>
            Pessoas
          </button>
          <button
            className={tab === 'transactions' ? 'active' : ''}
            onClick={() => setTab('transactions')}
          >
            Transações
          </button>
          <button className={tab === 'totals' ? 'active' : ''} onClick={() => setTab('totals')}>
            Totais
          </button>
        </nav>
      </header>

      <main className="app-content">
        {globalError && <p className="error-message global-error">{globalError}</p>}

        {loading ? (
          <p className="empty-state">Carregando...</p>
        ) : (
          <>
            {tab === 'people' && (
              <section>
                <PersonForm onSubmit={handleCreatePerson} />
                <PeopleList people={people} onDelete={handleDeletePerson} />
              </section>
            )}

            {tab === 'transactions' && (
              <section>
                <TransactionForm people={people} onSubmit={handleCreateTransaction} />
                <TransactionsList transactions={transactions} />
              </section>
            )}

            {tab === 'totals' && (
              <section>
                <TotalsView report={totals} />
              </section>
            )}
          </>
        )}
      </main>
    </div>
  );
}
