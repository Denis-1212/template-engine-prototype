import React, { useState } from 'react';
import './App.css';

const App: React.FC = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [middleName, setMiddleName] = useState('');
  const [template, setTemplate] = useState('');
  const [preview, setPreview] = useState('');
  const [processingType, setProcessingType] = useState<'Explicit' | 'Ai'>('Explicit');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async () => {
    if (!template.trim()) {
      setError('Шаблон не может быть пустым');
      return;
    }
    if (!firstName.trim() || !lastName.trim() || !middleName.trim()) {
      setError('Все поля ФИО обязательны для заполнения');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const response = await fetch('http://localhost:5135/api/template/process', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          firstName: firstName.trim(),
          lastName: lastName.trim(),
          middleName: middleName.trim(),
          template: template,
          processingType: processingType,
        }),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `Ошибка: ${response.status}`);
      }

      const data = await response.json();
      setPreview(data.preview);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Произошла ошибка');
    } finally {
      setLoading(false);
    }
  };


  return (
    <div className="app">
      <h1>Шаблонизатор документов</h1>

      <div className="container">
        <div className="form-section">
          <div className="input-group">
            <label>Фамилия *</label>
            <input
              type="text"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
              placeholder="Иванов"
              disabled={loading}
            />
          </div>

          <div className="input-group">
            <label>Имя *</label>
            <input
              type="text"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
              placeholder="Иван"
              disabled={loading}
            />
          </div>

          <div className="input-group">
            <label>Отчество *</label>
            <input
              type="text"
              value={middleName}
              onChange={(e) => setMiddleName(e.target.value)}
              placeholder="Иванович"
              disabled={loading}
            />
          </div>

          <div className="input-group">
            <label>Тип обработки</label>
            <div className="radio-group">
              <label>
                <input
                  type="radio"
                  value="Explicit"
                  checked={processingType === 'Explicit'}
                  onChange={() => setProcessingType('Explicit')}
                  disabled={loading}
                />
                Скрипт
              </label>
              <label>
                <input
                  type="radio"
                  value="Ai"
                  checked={processingType === 'Ai'}
                  onChange={() => setProcessingType('Ai')}
                  disabled={loading}
                />
                AI
              </label>
            </div>
          </div>

          <div className="input-group">
            <label>Шаблон *</label>
            <textarea
              value={template}
              onChange={(e) => setTemplate(e.target.value)}
              placeholder="Введите шаблон..."
              rows={10}
              disabled={loading}
            />
          </div>

          <button onClick={handleSubmit} disabled={loading}>
            {loading ? 'Обработка...' : 'Превью'}
          </button>

          {error && <div className="error">{error}</div>}
        </div>

        <div className="preview-section">
          <label>Результат</label>
          <textarea
            value={preview}
            readOnly
            placeholder="Результат обработки..."
            rows={10}
          />

          <div className="template-examples">
            <p className="example-title">Пример шаблона для скрипта:</p>
            <div className="example-code">
              <div>Привет, {'{{name.nom}}'}!</div>
              <div>Вижу {'{{lastname.acc}} {{name.acc}}.'}</div>
              <div>Помогаю {'{{name.dat}} {{middlename.dat}}.'}</div>
            </div>

            <p className="example-title">Пример шаблона для AI:</p>
            <div className="example-code">
              <div>Привет, {'{{name}}'}!</div>
              <div>Вижу {'{{name}} {{lastname}}.'}</div>
              <div>Помогаю {'{{name}} {{middlename}}.'}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default App;