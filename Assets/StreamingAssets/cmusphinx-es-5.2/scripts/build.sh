#!/bin/sh

download() {
cd tgz

wget -N -nd -c -e robots=off -A tgz,html -r -np \
 http://www.repository.voxforge1.org/downloads/es/Trunk/Audio/Main/16kHz_16bit
# http://www.repository.voxforge1.org/downloads/SpeechCorpus/Trunk/Audio/Main/8kHz_16bit

cd ..
}

unpack() {
for f in tgz/*.tgz; do
    tar xf $f -C wav
done
}

convert_flac() {
find  wav -name "*flac*" -type d  | while read file; do
    outdir=${file//flac/wav}
    mkdir -p $outdir
done
find  wav -name "*.flac"  | while read f; do
    outfile=${f//flac/wav}
    flac -s -d $f -o $outfile
done
}

collect_prompts() {
mkdir etc
> etc/allprompts
find wav -name PROMPTS | while read f; do
    echo $f
    cat $f >> etc/allprompts
done
#find wav -name prompts | while read f; do
#    echo $f
#    cat $f >> etc/allprompts
#done
}

#FIXME
make_prompts() {
cat etc/allprompts | sort | sed 's/mfc/wav/g' |
mv allprompts.tmp etc/allprompts
cat etc/allprompts | awk '{
    if (system("test -f wav/" $1 ".wav") == 0) {
    printf ("<s> ");
    for (i=2;i<=NF;i++)
	printf ("%s ", tolower($i));
    printf ("</s> (%s)\n", $1);
    }
}
' > etc/voxforge_es_sphinx.transcription
./traintest.sh etc/voxforge_es_sphinx.transcription
./build_fileids.py etc/voxforge_es_sphinx.transcription.train > etc/voxforge_es_sphinx.fileids.train
./build_fileids.py etc/voxforge_es_sphinx.transcription.test > etc/voxforge_es_sphinx.fileids.test
}

#download
#unpack
#collect_prompts
#make_prompts
#sphinxtrain run
