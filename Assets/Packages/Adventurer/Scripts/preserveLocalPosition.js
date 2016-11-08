private var boneLocalPosition : Vector3[];
private var boneLocalScale : Vector3[];
var skinnedMeshRenderer : SkinnedMeshRenderer;

function Start(){
	boneLocalPosition = new Vector3[skinnedMeshRenderer.bones.Length];
	boneLocalScale = new Vector3[skinnedMeshRenderer.bones.Length];
	for(var i = 0; i < skinnedMeshRenderer.bones.Length; i++){
		boneLocalPosition[i] = skinnedMeshRenderer.bones[i].localPosition;
		boneLocalScale[i] = skinnedMeshRenderer.bones[i].localScale;
	}
}
function LateUpdate () {
	for(var i = 0; i < skinnedMeshRenderer.bones.Length; i++){
		skinnedMeshRenderer.bones[i].localPosition = boneLocalPosition[i];
		skinnedMeshRenderer.bones[i].localScale = boneLocalScale[i];
	}
}